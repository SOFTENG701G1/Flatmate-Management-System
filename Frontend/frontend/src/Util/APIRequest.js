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

    //Retrieves the list of flat memebrs in the current users flat
    static async getFlatMembers(){
        let authString = await APIRequest.getAuthString();
        let res = await fetch(apiBaseUrl + "api/members/getmembers",
        {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': authString
            },
            method: "GET",
            //mode: "no-cors",

        });

        return res
    }

    //Gets the server to create a new flat for the user.
    static async createNewFlat(){
        let authString = await APIRequest.getAuthString();
        let res = await fetch(apiBaseUrl + "api/members/createFlat",
        {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': authString
            },
            method: "POST",
            //body: JSON.stringify({ address: address })
        });
        

        return res
    }
}