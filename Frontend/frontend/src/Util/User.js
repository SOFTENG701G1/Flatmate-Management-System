export default class User {
    static userState = localStorage.getItem("flatmates_app_user") ?
        JSON.parse(localStorage.getItem("flatmates_app_user")) : undefined;
    
    static setUserState(newState) {
        this.userState = newState;

        localStorage.setItem("flatmates_app_user", JSON.stringify(newState));
    }

    static getUserState() {
        return this.userState;
    }
}