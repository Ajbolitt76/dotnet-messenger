import {
  _NotCustomized,
  IAnyModelType,
  IAnyType,
  IModelType,
  Instance,
  ModelProperties,
  ModelPropertiesDeclarationToProperties,
  types
} from "mobx-state-tree";
import { ExtractCFromProps, ExtractOthers, ExtractProps, model } from "mobx-state-tree/dist/types/complex-types/model";
import { action, computed, IComputedValue, makeObservable, observable } from "mobx";
import toPairs from "lodash/toPairs";
import values from "lodash/values";
import { isAnyError } from "@/lib/validators/validarionUtils";

export type ValueValidationResult<TValue> =
  | (TValue extends Array<infer TEachValue>
  ? Array<ValueValidationResult<TEachValue>> | string | null
  : TValue extends object
    ?
    | {
    [propertyName in keyof TValue]?: ValueValidationResult<
      TValue[propertyName]
    >;
  }
    | string
    | null
    : string | null)
  | string
  | null;

type ValueValidatorFunction<TModel, TValue> = (
  value: TValue,
  model: TModel,
) => ValueValidationResult<TValue>;

type ValidatorDefinition<TModel, TValue> = {
  validate: ValueValidatorFunction<TModel, TValue>;
  isDelayed: boolean;
}

type ValueValidator<TModel, TValue> =
  | ValueValidatorFunction<TModel, TValue>
  | ValidatorDefinition<TModel, TValue>;

type Validators<T> = {
  [K in keyof T]?: Array<ValueValidator<T, T[K]>>;
};

type NormalizedValidators<T> = {
  [K in keyof T]?: Array<ValidatorDefinition<T, T[K]>>;
};

type ValidationErrors<T> = {
  [K in keyof T]?: () => ValueValidationResult<T[K]>[];
}

type ValidatorProps<T> = {
  isValid: boolean;
  errors: ValidationErrors<T>;
};

export type DynamicValidationErrors<TModel> = {
  [propertyName in keyof TModel]?: IComputedValue<ValueValidationResult<TModel[propertyName]>[]>;
};

export type StaticValidationErrors<TModel> = {
  [propertyName in keyof TModel]?: ValueValidationResult<TModel[propertyName]>[];
};

type Validatable = {
  [key: string]: unknown;
}


class Validator<T extends Validatable> {
  private _validationErrors: DynamicValidationErrors<T> = {};
  private readonly _model: T;
  private readonly _validators: NormalizedValidators<T>;

  @observable
  private readonly _staticValidationErrors: StaticValidationErrors<T> = {};

  @observable
  private _isPaused: boolean = false;

  constructor(model: T, validators: Validators<T>) {
    this._model = model;
    this._validators = this.normalizeValidators(validators);
    this.buildValidators();
    makeObservable(this);
  }

  @computed
  get isValid() {
    return !isAnyError(values(this._validationErrors).flatMap((x) => x?.get() ?? []));
  }

  get errors() {
    return this._validationErrors;
  }

  @computed
  get isPaused(): boolean {
    return this.isPaused;
  }

  @action
  public pause(): void {
    this._isPaused = true;
  }

  @action
  public resume(): void {
    this._isPaused = false;
  }

  private normalizeValidators(validators: Validators<T>): NormalizedValidators<T> {
    return toPairs(validators).reduce((acc, [key, value]) => {
      const propertyName = key as keyof T;
      const validatorList = value as Array<ValueValidator<T, T[keyof T]>>;
      acc[propertyName] = validatorList.map((validator) => {
        if (typeof validator === "function") {
          return { validate: validator, isDelayed: false };
        }
        return validator;
      });
      return acc;
    }, {} as NormalizedValidators<T>);
  }

  private makeValidator<TKey extends keyof T>(validators: ValidatorDefinition<T, T[TKey]>[], propertyName: TKey)
    : IComputedValue<ValueValidationResult<T[TKey]>[]> {
    const dynamicValidators = validators.filter((x) => !x.isDelayed);
    return computed(() => {
      if (this._isPaused) {
        return [];
      }

      const value = this._model[propertyName];
      const dynamicResults = dynamicValidators
        ?.map((validator) => {
          if (validator.isDelayed) {
            return null;
          }
          return validator.validate(value, this._model);
        }) ?? [];
      return dynamicResults.concat(this._staticValidationErrors[propertyName] ?? [])
    }, {
      name: `validator_${String(propertyName)}`,
    })
  }

  @action
  public validate(): void {
    this.resume();
    const validationPairs = toPairs(this._validators);
    for (const [propertyString, validatorList] of validationPairs
      ) {
      if (!validatorList) {
        continue;
      }
      const castedValidators = validatorList as ValidatorDefinition<T, T[keyof T]>[];
      const propertyName = propertyString as keyof T;
      const value = this._model[propertyName];

      this._staticValidationErrors[propertyName] = castedValidators
        .filter((x) => x.isDelayed)
        .map((validator) => {
          if (!validator.isDelayed) {
            return null;
          }
          return validator.validate(value, this._model);
        }) ?? [];
    }
  }

  private buildValidators(): void {
    const validationPairs = toPairs(this._validators);
    for (const [propertyString, validatorList] of validationPairs
      ) {
      if (!validatorList) {
        continue;
      }

      const propertyName = propertyString as keyof T;
      this._validationErrors[propertyName] = this.makeValidator(validatorList, propertyString);
    }
  }
}

interface ModelTypes<T extends IAnyModelType> {
  PROPS: T extends IModelType<infer P, any, any, any> ? P : never;
  OTHERS: T extends IModelType<any, infer O, any, any> ? O : never;
  CustomC: T extends IModelType<any, any, infer C, any> ? C : never;
  CustomS: T extends IModelType<any, any, any, infer S> ? S : never;
}

type ExtendedModel<T extends IAnyModelType, OtherExtensions extends {}, PropExtensions extends {} = {}>
  = IModelType<ModelTypes<T>["PROPS"] & PropExtensions, ModelTypes<T>["OTHERS"] & OtherExtensions, ModelTypes<T>["CustomC"], ModelTypes<T>["CustomS"]>

export function addValidator<
  T extends IAnyModelType
>(
  storeType: T,
  validators: Validators<Instance<T>>,
  startPaused: boolean = false): ExtendedModel<T, { get validator(): Validator<Instance<T>>; }> {
  return storeType.views(self => {
    const validator = new Validator(self, validators);

    if (startPaused) {
      validator.pause();
    }

    return {
      get validator() {
        return validator;
      }
    };
  });

}
