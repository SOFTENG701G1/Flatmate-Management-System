export default class User {
    
    static setUserState(newState) {
        this.userState = newState;

        localStorage.setItem("flatmates_app_user", JSON.stringify(newState));
    }

    static clearUserState(newState) {
        this.userState = undefined;

        localStorage.removeItem("flatmates_app_user");
    }

    static getUserState() {
        return this.userState;
    }

    static setFlatState(newState){
        this.flatState = newState;
    }

    static getFlatState(){
        return this.flatState;
    }
}

try {
    User.flatState = {"flatMembers": []};
    User.userState = localStorage.getItem("flatmates_app_user") ?
        JSON.parse(localStorage.getItem("flatmates_app_user")) : undefined;
} catch (e) {}