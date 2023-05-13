import React, { useState } from 'react'
import reactLogo from './assets/react.svg'
import { QueryClientProvider } from "@tanstack/react-query"
import { queryClient } from './lib/ReactQuery'
import { AdaptivityProvider, AppRoot, Button, ConfigProvider, Spinner } from "@vkontakte/vkui";
import { ErrorBoundary } from "react-error-boundary";
import { HelmetProvider } from 'react-helmet-async';
import { AppRouter } from "./routes";
import { AuthProvider } from "@/lib/AuthProvider";
import { Notifications } from "@/components/Notifications";
import { StoreProvider } from "@/stores/RootStore";
import { ErrorFallback } from "@/components/ErrorFallback";
import { ModalProvider } from './components/Modal'

import './App.css'
import "@vkontakte/vkui/dist/vkui.css";
import { RealtimeRoot } from './features/realtime/components/RealtimeRoot';

function App() {
  return (
    <React.Suspense
      fallback={
        <div className="flex items-center justify-center w-screen h-screen">
          123
          {/*<Spinner size="large"/>*/}
        </div>
      }
    >
      <ErrorBoundary FallbackComponent={ErrorFallback}>
        <HelmetProvider>
          <StoreProvider>
            <ConfigProvider appearance="dark">
              <AdaptivityProvider>
                <AppRoot mode="partial">
                  <QueryClientProvider client={queryClient}>
                    <ModalProvider>
                      <AuthProvider>
                        <RealtimeRoot />
                        <Notifications />
                        <AppRouter />
                      </AuthProvider>
                    </ModalProvider>
                  </QueryClientProvider>
                </AppRoot>
              </AdaptivityProvider>
            </ConfigProvider>
          </StoreProvider>
        </HelmetProvider>
      </ErrorBoundary>
    </React.Suspense>
  )
}

export default App
