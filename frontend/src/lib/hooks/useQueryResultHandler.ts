import { useStore } from "@/stores/RootStore";
import { TgApiError } from "../ApiClient";
import { isFunction } from "lodash";

export function useQueryResultHandler<TData = unknown>(
  errorMessageTitle: string | ((x: TgApiError) => string),
  okMessage?: string | ((x: TData) => string),
) {
  const { notificationStore } = useStore();

  return {
    handleError: async (error: unknown) => {
      var parsedError = await TgApiError.TryGetFromCaught(error);
      const title = parsedError
        ? (isFunction(errorMessageTitle) ? errorMessageTitle(parsedError) : errorMessageTitle)
        : "Ошибка"
      notificationStore.error(parsedError?.title ?? "Неизвестная ошибка", title);
    },
    handleOk: (data: TData) => {
      if (!okMessage)
        return;
      const text = isFunction(okMessage) ? okMessage(data) : okMessage;
      notificationStore.success("Успех", text);
    }
  }
}