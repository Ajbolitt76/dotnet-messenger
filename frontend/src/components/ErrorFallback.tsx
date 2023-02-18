import { Button } from "@vkontakte/vkui";

export const ErrorFallback = () => {
  return (
    <div
      className="text-red-500 w-screen h-screen flex flex-col justify-center items-center"
      role="alert"
    >
      <h2 className="text-lg font-semibold">Неудалось загрузиться :( </h2>
      <Button className="mt-4" onClick={() => window.location.assign(window.location.toString())}>
        Поаторить попытку
      </Button>
    </div>
  );
};
