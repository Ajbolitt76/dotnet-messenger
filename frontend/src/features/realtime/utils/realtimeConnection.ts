import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Reducer, useEffect, useReducer, useState } from 'react';

class RealtimeConnection {
  private _initialized = false;
  private _accessToken?: string;
  public readonly connection: HubConnection;

  public get isInitialized(): boolean {
    return this._initialized;
  }

  constructor() {
    this.connection =
      new HubConnectionBuilder()
        .withUrl(`${import.meta.env.VITE_API_URL}realtime`, { accessTokenFactory: () => this._accessToken! })
        .withAutomaticReconnect()
        .build();
  }

  public setAccessToken(token: string): void {
    this._accessToken = token;
  }

  public async start(): Promise<void> {
    await this.connection.start();
  }
}

export const realtimeConnection = new RealtimeConnection();

enum ReducerActionNames {
  StatusChanged = "STATUS_CHANGED",
  ConnectionFailed = "CONNECTION_FAILED"
}

type ReducerAction = { type: ReducerActionNames.StatusChanged } | { type: ReducerActionNames.ConnectionFailed, error: Error }

type ReducerState = {
  status: HubConnectionState,
  isConnected: boolean,
  isLoading: boolean,
  isFailed: boolean,
}

const onStateChangedAction: ReducerAction = {
  type: ReducerActionNames.StatusChanged
}

const onFailedAction = (e: Error) => ({
  type: ReducerActionNames.ConnectionFailed,
  error: e
}) as ReducerAction

export function useRealtimeConnectionStatus(): [state: ReducerState, onStateChange: () => void, onFailed: (e: Error) => void] {
  const signal = realtimeConnection.connection;

  const [state, dispatch] = useReducer<Reducer<ReducerState, ReducerAction>, null>(
    (state, action) => {
      switch (action.type) {
        case ReducerActionNames.StatusChanged:
          return {
            status: signal.state,
            isConnected: signal.state == HubConnectionState.Connected,
            isLoading: signal.state == HubConnectionState.Connecting
              || signal.state == HubConnectionState.Reconnecting
              || signal.state == HubConnectionState.Disconnecting,
            isFailed: false
          }
        case ReducerActionNames.ConnectionFailed:
          return {
            status: signal.state,
            isConnected: false,
            isLoading: false,
            isFailed: true
          }
        default:
          return state;
      }
    },
    null,
    (a: null) => ({
      status: signal.state,
      isConnected: signal.state == HubConnectionState.Connected,
      isLoading: signal.state == HubConnectionState.Connecting
        || signal.state == HubConnectionState.Reconnecting
        || signal.state == HubConnectionState.Disconnecting,
      isFailed: false
    }));

  useEffect(() => {
    function onStatusChange() {
      dispatch(onStateChangedAction);
    }

    signal.onclose(onStatusChange)
    signal.onreconnecting(onStatusChange)
    signal.onreconnected(onStatusChange)
  }, []);

  return [state, () => dispatch(onStateChangedAction), (e: Error) => dispatch(onFailedAction(e))];
}