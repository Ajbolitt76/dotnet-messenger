import ky from 'ky';

class TokenManager {
    private token: string | null = null;
    constructor(){
        this.token = localStorage.getItem('token');
    }
    public set Token(token: string | null){
        if(token == null)
            localStorage.removeItem('token');
        else
            localStorage.setItem('token', token);

        this.token = token;
    }
    public get Token(){
        return this.token;
    }
}


export const apiClient = ky.extend({
  prefixUrl: import.meta.env.VITE_API_URL || undefined,
	hooks: {
		beforeRequest: [
			request => {
				request.headers.set('Authorization', `Bearer ${tokenStore.Token}`);
			}
		]
	}
});

export const tokenStore = new TokenManager();
