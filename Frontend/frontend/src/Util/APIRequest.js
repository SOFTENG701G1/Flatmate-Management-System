import User from "./User";

const apiBaseUrl = process.env.REACT_APP_BACKEND_API

export default class APIRequest {
    static async login(username, password) {
        let res = await fetch(apiBaseUrl + "api/user/login",
        {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({ username: username, password: password })
        });

        return res;
    }

    static async getAuthString() {
        if (!User.getUserState()) {
            return undefined;
        }

        return 'Bearer ' + User.getUserState().token;
    }
}