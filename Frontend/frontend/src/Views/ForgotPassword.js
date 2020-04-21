import React from "react";
import { Redirect, Link } from "react-router-dom";

import "./Login.css";
import APIRequest from "../Util/APIRequest";
import User from "../Util/User";
import Logo from "../images/logo-house-blue.png";
import BackArrow from "../images/back-arrow.png";

export default class ForgotPassword extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      loginIdentifier: undefined, // Username or email
      isEmailSent: false,
    };

    this.sendInstructions = this.sendInstructions.bind(this);
    this.bindInput = this.bindInput.bind(this);
  }

  async sendInstructions(event) {
    const { loginIdentifier } = this.state;
    event.preventDefault();
    this.setState({ sendingRequest: false });
    this.setState({ error: "" });
    if (!loginIdentifier) {
      this.setState({ error: "Enter a username or email address." });
      return;
    }
    this.setState({ sendingRequest: true });
    // Call the API to send a reset password E-mail to the given account E-mail address
    const sendInstructionsResult = await APIRequest.forgotPassword(
      loginIdentifier
    );
    this.setState({ sendingRequest: false });
    if (sendInstructionsResult.ok) {
      this.setState({ isEmailSent: true });
    } else {
      switch (sendInstructionsResult.status) {
        case 404:
          this.setState({
            error: "There is no account with that username or email.",
          });
          break;
        default:
          this.setState({ error: "Unknown error (check your internet)." });
          break;
      }
    }
  }

  bindInput(event) {
    const {target} = event;

    this.setState({
      [target.name]: target.value,
    });
  }

  render() {
    const { sendingRequest } = this.state;
    const { isEmailSent } = this.state;
    const { error } = this.state;
    return (
      <div className="login-backdrop">
        {User.getUserState() ? <Redirect to="/app/" /> : ""}
        <div className="login-container">
          <Link to="/login">
            <img src={BackArrow} alt="Go Back" className="back-arrow" />
          </Link>
          <img src={Logo} alt="Logo" className="logo-image" />
          <h2>
            {isEmailSent
              ? "We've sent your instructions."
              : "Forgot your password?"}
          </h2>
          {isEmailSent ? (
            <>
              <p className="instructions-text">
                An email should be sent to you shortly. Please follow the instructions to reset your password.
              </p>
              <Link to="/login">
                <input type="button" value="Return to login" />
              </Link>
            </>
          ) : (
            <form action="#" method="POST">
              <p className="instructions-text">
                {" "}
                {
                  "Enter your email or username and we'll email you with instructions to reset your password."
                }
              </p>
              <input
                type="text"
                name="loginIdentifier"
                onChange={this.bindInput}
                placeholder="Email or username"
              />
              {!sendingRequest ? (
                <input
                  type="submit"
                  value="Send instructions"
                  onClick={this.sendInstructions}
                />
              ) : null}
              {error ? (
                <div className="login-error"> {error} </div>
              ) : (
                <div className="error-placeholder" />
              )}
            </form>
          )}
        </div>
      </div>
    );
  }
}
