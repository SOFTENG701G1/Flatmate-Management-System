import User from "./User";

const apiBaseUrl = process.env.REACT_APP_BACKEND_API

/**
 * This class provides helper functions for API requests, and provides auth for those requests.
 */
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

    // Gets the auth string, which can be added to header "Authorization" for requests that need auth
    static async getAuthString() {
        if (!User.getUserState()) {
            return undefined;
        }

        return 'Bearer ' + User.getUserState().token;
    }
}