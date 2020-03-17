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

    //Checks if the username already exists in the database
    static async checkNewAccount(username, email) {
        let res = await fetch(apiBaseUrl + "api/user/check",
        {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({ username: username, email: email})
        });

        return res;
    }

    static async register(username, firstName, lastName, dateOfBirth, phoneNumber,
        email, medicalInfo, bankAccountNumber, password) {
        let res = await fetch(apiBaseUrl + "api/user/register",
        {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({ username: username, firstname: firstName, lastname: lastName,
                                    dateofbirth: dateOfBirth, phonenumber: phoneNumber, email: email,
                                    medicalinformation: medicalInfo, bankAccount: bankAccountNumber, password: password})
        });

        return res;
    }
    static async componentDidMount(){
        let authString = await APIRequest.getAuthString();
        const res = await fetch(apiBaseUrl + "api/flat/display",{
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization':  authString
            },
            method: "GET",
        })
        return res;
    }
}