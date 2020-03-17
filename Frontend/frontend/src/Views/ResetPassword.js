import React from 'react';
import { Redirect, Link } from 'react-router-dom';

import './Login.css';
import APIRequest from '../Util/APIRequest';
import User from '../Util/User';
import Logo from '../images/logo-house-blue.png';
import BackArrow from '../images/back-arrow.png';

export default class ResetPassword extends React.Component {
  constructor (props) {
    super(props);

    this.state = {
      password: undefined,
      passwordRepeat: undefined,
      isPasswordReset: false,
    }

    this.resetPassword = this.resetPassword.bind(this);
    this.bindInput = this.bindInput.bind(this);
  }

  async resetPassword (event) {
    event.preventDefault();
    this.setState({ error: "" });
    if (!this.state.password) {
      this.setState({ error: "Please enter a new password." });
      return;
    }
    if (!this.state.passwordRepeat) {
      this.setState({ error: "Please re-enter your new password." });
      return;
    }
    if (this.state.password !== this.state.passwordRepeat) {
      this.setState({ error: "Passwords do not match." });
      return;
    }

    // TODO: Remove this line once API requests are set up
    this.setState({ isPasswordReset: true });

    // TODO: Remove once API requests are set up
    // let resetPasswordResult = await APIRequest.resetPassword(this.state.password);

    // if (resetPasswordResult.ok) {
    //   this.setState({ isPasswordReset: true });
    // } else {
    //   // TODO: Change switch statements to display correct errors
    //   switch (resetPasswordResult.status) {
    //     case 404:
    //       this.setState({ error: "Username does not exist." });
    //       break;
    //     case 403:
    //       this.setState({ error: "Invalid password."});
    //       break;
    //     default:
    //       this.setState({ error: "Unknown error (check your internet)."});
    //       break;
    //   }
    // }
  }
  
  bindInput (event) {
    let target = event.target;

    this.setState({
      [target.name]: target.value
    });
  }

  render() {
    return (
      <div className="login-backdrop">
        { User.getUserState() ? <Redirect to="/app/"/> : '' }
        <div className='login-container'>
          <Link to="/home"><img src={BackArrow} alt="Go Back" className="back-arrow"/></Link>
          <img src={Logo} alt="Logo" className="logo-image"/>
          <h2>{this.state.isPasswordReset ? "Password has been reset" : "Reset your password"}</h2>
          { this.state.isPasswordReset ? 
            <Link to="/login"><input type='button' value='Login'/></Link>
            : 
            <form action="#" method="POST">
              <p className="instructions-text">Please enter a new password.</p>
              <input type='password' name='password' className="input-field" onChange={this.bindInput} placeholder='New password'/>
              <input type='password' name='passwordRepeat' className="input-field" onChange={this.bindInput} placeholder='Retype new password'/>
              <input type='submit' value='Reset password' onClick={this.resetPassword}/>
              { this.state.error ? <div className='login-error'> { this.state.error } </div> : <div className="error-placeholder"/> }
            </form>
          }
        </div>
      </div>
    );
  }
}