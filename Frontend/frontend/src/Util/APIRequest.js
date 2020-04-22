import User from "./User";

const apiBaseUrl = process.env.REACT_APP_BACKEND_API;
/**
 * This class provides helper functions for API requests, and provides auth for those requests.
 */
export default class APIRequest {
  static async login(username, password) {
    let res = await fetch(apiBaseUrl + "api/user/login", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({ username: username, password: password }),
    });

    return res;
  }

  // Reset password with the given E-mail and password. It is not using username because the reset link was accessed via user's E-mail
  static async resetPassword(email, password) {
    let res = await fetch(apiBaseUrl + "api/user/resetPassword", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({ username: email, password: password }),
    });

    return res;
  }

  // Validate if user is in the system and send an E-mail for resetting password
  static async forgotPassword(userOrEmail) {
    let res = await fetch(apiBaseUrl + "api/user/forgotPassword", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({ userOrEmail: userOrEmail }),
    });

    return res;
  }

  // Check if the reset token given was created less than 1 hour ago and if it's valid
  // This needs to be executed prior to page rendering, so an async method is used
  static checkResetToken(email, resetToken) {
    let res = fetch(apiBaseUrl + "api/user/resetTokenCheck", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({ Email: email, ResetToken: resetToken }),
    });

    return res;
  }

  // Gets the auth string, which can be added to header "Authorization" for requests that need auth
  static async getAuthString() {
    if (!User.getUserState()) {
      return undefined;
    }

    return "Bearer " + User.getUserState().token;
  }

  //Checks if the username already exists in the database
  static async checkNewAccount(username, email) {
    let res = await fetch(apiBaseUrl + "api/user/check", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({ username: username, email: email }),
    });

    return res;
  }

  //Retrieves the user id of the current user
  static async getUserID() {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + "api/user/getUserID", {
      header: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "GET",
    });
    return res;
  }

  static async register(
    username,
    firstName,
    lastName,
    dateOfBirth,
    phoneNumber,
    email,
    medicalInfo,
    bankAccountNumber,
    password
  ) {
    let res = await fetch(apiBaseUrl + "api/user/register", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({
        username: username,
        firstname: firstName,
        lastname: lastName,
        dateofbirth: dateOfBirth,
        phonenumber: phoneNumber,
        email: email,
        medicalinformation: medicalInfo,
        bankAccount: bankAccountNumber,
        password: password,
      }),
    });

    return res;
  }

  static async createNewPayment(
    amount,
    startDate,
    paidTo,
    endDate,
    account,
    frequency,
    contributorsPending,
    contributorsPaid,
    description,
    listOfIds
  ) {
    let authString = await APIRequest.getAuthString();
    console.log("the ids in the ap are" + listOfIds);
    let res = await fetch(apiBaseUrl + "api/Payments/Flat", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
        userIds: listOfIds,
      },
      method: "POST",
      body: JSON.stringify({
        amount: amount,
        startDate: startDate,
        paidTo: paidTo,
        endDate: endDate,
        account: account,
        frequency: frequency,
        contributorsPending: contributorsPending,
        contributorsPaid: contributorsPaid,
        description: description,
      }),
    });
    console.log(res);
    return res;
  }

  static async obtainUserPayments() {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + "api/Payments/User", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "GET",
    }).then((data) => {
      if (data.ok) return data.json();
    });

    return res;
  }

  static async obtainPaymentContributors(paymentId) {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(
      apiBaseUrl + `api/Payments/Users?paymentId=${paymentId}`,
      {
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
          Authorization: authString,
        },
        method: "GET",
      }
    ).then((data) => {
      if (data.ok) return data.json();
    });

    return res;
  }

  static async componentDidMount() {
    let authString = await APIRequest.getAuthString();
    const res = await fetch(apiBaseUrl + "api/flat/display", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "GET",
    });
    return res;
  }

  //Retrieves the list of flat members in the current users flat
  static async getFlatMembers() {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + "api/flat/getmembers", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "GET",
      //mode: "no-cors",
    });

    return res;
  }

  //Gets the server to create a new flat for the user.
  static async createNewFlat() {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + "api/flat/createFlat", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "POST",
    });

    return res;
  }

  // Get ids of specified users
  static async getUserIdsByUsername(UserID) {
    let res = await fetch(apiBaseUrl + "api/User/getUsersIds", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({ UserID: UserID }),
    }).then((data) => {
      if (data.ok) {
        return data.json();
      }
    });
    return res;
  }

  static async createChore(chore) {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + "api/chore/Flat", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "POST",
      body: JSON.stringify(chore),
    });
    return res;
  }

  static async getChoresForFlat() {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + "api/chore/Flat", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "GET",
    });
    return res;
  }

  static async deleteChore(id) {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + `api/Chore/${id}`, {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "DELETE",
    });
    return res;
  }
  //Gets the account information of the current user
  static async getUserInfo() {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + "api/User/getUserInfo", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "POST",
    });

    return res;
  }
  static async editUserInfo(
    firstName,
    lastName,
    dateOfBirth,
    phoneNumber,
    email,
    medicalInfo,
    bankAccountNumber
  ) {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + "api/User/editUserInfo", {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "POST",
      body: JSON.stringify({
        firstname: firstName,
        lastname: lastName,
        dateofbirth: dateOfBirth,
        phonenumber: phoneNumber,
        email: email,
        medicalinformation: medicalInfo,
        bankAccount: bankAccountNumber,
      }),
    });

    return res;
  }

  static async addMember(username) {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + `api/Flat/AddUsertoFlat/${username}`, {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "GET",
    });
    return res;
  }

  static async markChoreComplete(id) {
    let authString = await APIRequest.getAuthString();
    let res = await fetch(apiBaseUrl + `api/Chore/Chores/${id}`, {
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: authString,
      },
      method: "PUT",
    });
    return res;
  }
}
