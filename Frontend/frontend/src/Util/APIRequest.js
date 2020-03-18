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

    static async obtainUserPayments(){
        let authString = await APIRequest.getAuthString();
        let res = await fetch(apiBaseUrl + "api/Payments/User", {
            headers:{
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization':  authString
            },
            method: "GET",
        }).then(
            data => {
                if(data.ok) return data.json()
            }
        )

        return res;
    }

    static async obtainPaymentContributors(paymentId){
        let authString = await APIRequest.getAuthString();
        let res = await fetch(apiBaseUrl + `api/Payments/Users?paymentId=${paymentId}`, {
            headers:{
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization':  authString
            },
            method: "GET",
        }).then(
            data => {
                if(data.ok) return data.json()
            }
        )

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

    //Retrieves the list of flat memebrs in the current users flat
    static async getFlatMembers(){
        let authString = await APIRequest.getAuthString();
        let res = await fetch(apiBaseUrl + "api/flat/getmembers",
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
        let res = await fetch(apiBaseUrl + "api/flat/createFlat",
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