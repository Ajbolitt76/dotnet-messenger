import { LoginResponseDto } from '@/features/user/api/login';
import ky, { HTTPError } from 'ky';
import { ApiError } from './ApiTypes';

class TokenManager {
  private token?: string | null = null;
  private refreshToken?: string | null = null;

  constructor() {
    this.token = localStorage.getItem('token');
    this.refreshToken = localStorage.getItem('refreshToken');
  }

  public get RefreshToken() {
    return this.refreshToken;
  }

  public get Token() {
    return this.token;
  }

  public UpsertFromDto(dto?: LoginResponseDto) {
    this.token = dto?.token;
    this.lsSetOrRemove("token", dto?.token);
    this.refreshToken = dto?.refreshToken;
    this.lsSetOrRemove("refreshToken", dto?.refreshToken);
  }

  private lsSetOrRemove(key: string, value?: string | null) {
    if (value == null)
      localStorage.removeItem(key);
    else
      localStorage.setItem(key, value);
  }
}


export const apiClient = ky.extend({
  prefixUrl: import.meta.env.VITE_API_URL || undefined,
  hooks: {
    beforeRequest: [
      request => {
        if(tokenStore.Token)
          request.headers.set('Authorization', `Bearer ${tokenStore.Token}`);
      }
    ],
    afterResponse: [
      async (req, opt, res) => {
        if (res.status === 401 && !req.url.includes("auth/refresh")) {
          var result = await apiClient.post("auth/refresh", {
            json: {
              token: tokenStore.Token,
              refreshToken: tokenStore.RefreshToken,
            }
          }).json<LoginResponseDto>();
          tokenStore.UpsertFromDto(result);
          req.headers.set('Authorization', `Bearer ${tokenStore.Token}`);
          return apiClient(req);
        }else if(res.status === 401){
          tokenStore.UpsertFromDto(undefined);
        }
      }
    ]
  }
});

export class TgApiError implements ApiError{
  public readonly title: string;
  public readonly status: number;
  public readonly placeholder: Record<string, string>;
  public readonly traceId: string;
  public readonly innerError: HTTPError;

  private constructor(error: HTTPError, data: ApiError){
    this.innerError = error;
    this.title = data.title;
    this.status = data.status;
    this.traceId = data.traceId;
    this.placeholder = data.placeholder;
  }

  public static async TryGetFromCaught(x: unknown): Promise<TgApiError | undefined>{
    if(!(x instanceof HTTPError))
      return;

    try{
      var data = await x.response.json()
      if(data.title != null)
        return new TgApiError(x, data);
    }catch(e){
      return;
    }
  }
}

export const tokenStore = new TokenManager();
