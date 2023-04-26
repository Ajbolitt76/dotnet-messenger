import { Instance, types } from 'mobx-state-tree';
import { InitiatePhoneAuthResponseDto } from '../api/inititatePhoneAuth';

const PhoneTicketModel = types.model("PhoneTicket", {
    phone: types.maybe(types.string),
    ticket: types.maybe(types.string),
    isLogin: types.maybe(types.boolean),
    nextTry: types.maybe(types.Date)
})
    .views(self => ({
        get hasActiveLoginTicket(){
            return self.isLogin && self.ticket != null
        },
        get hasActiveRegistrationTicket(){
            return self.isLogin === false && self.ticket != null
        }
    })).actions(self => ({
        setPhone(phone: string) {
            self.phone = phone;
        },
        updateFromInitiate(data: InitiatePhoneAuthResponseDto) {
            self.isLogin = data.isLogin;
            self.nextTry = new Date(data.nextTry);
        },
        setTicket(ticket: string) {
            self.ticket = ticket;
        },
        reset() {
            self.ticket = undefined;
            self.isLogin = undefined;
            self.nextTry = undefined;
            self.phone = undefined;
        }
    }));

export const AuthStore = types.model("Auth", {
    phoneTicket: types.optional(PhoneTicketModel, {})
});

export type AuthStoreInstance = Instance<typeof AuthStore>