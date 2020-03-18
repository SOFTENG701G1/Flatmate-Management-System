import React from 'react';
import { Redirect, Link } from 'react-router-dom';

import './Login.css';
import APIRequest from '../Util/APIRequest';
import User from '../Util/User';
import Logo from '../images/logo-house-blue.png';
import BackArrow from '../images/back-arrow.png';
import queryString from 'query-string';

export default class ResetPassword extends React.Component {
  constructor (props) {
    super(props);

    this.state = {
      password: undefined,
      passwordRepeat: undefined,
      isPasswordReset: false,
      error: null,
      isLoaded: false,
      LoadInvalidated: false,
      LoadValidated: false
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
    // this.setState({ isPasswordReset: true });

    // TODO: Remove once API requests are set up
    var values = queryString.parse(this.props.location.search)
    let resetPasswordResult = await APIRequest.resetPassword(values.email, this.state.password);
    
    if (resetPasswordResult.ok) {
      this.setState({ isPasswordReset: true });
    } else {
      // TODO: Change switch statements to display correct errors
      switch (resetPasswordResult.status) {
        case 404:
          this.setState({ error: "Username does not exist." });
          break;
        case 403:
          this.setState({ error: "Invalid password."});
          break;
        default:
          this.setState({ error: "Unknown error (check your internet)."});
          break;
      }
    }
  }
  
  bindInput (event) {
    let target = event.target;

    this.setState({
      [target.name]: target.value
    });
  }
  
  // Parse the query string and check if the inputs are valid before render
  componentDidMount() {
    // Base 64 string 
    var values = queryString.parse(this.props.location.search)
    values.token = values.token.split(' ').join('+');
    
    // Cannot use await outside of async function
    let resetTokenCheckdResult = APIRequest.checkResetToken(values.email, values.token)
    resetTokenCheckdResult.then(
      (result) => {
        // var tempProps = JSON.parse(JSON.stringify(this.props));
        console.log(result)
        if (result.ok){
          this.setState({
            LoadValidated: true
          });
        }
        else{
          this.setState({
            LoadInvalidated: true
          });
        }
        // Object.preventExtensions(tempProps);
        // Current proble - how to pass the validation result (bool) to render
        // console.log(this.props.validToken);
        // this.setState({
          // isLoaded: true,
        // });
      },
      // Note: it's important to handle errors here
      // instead of a catch() block so that we don't swallow
      // exceptions from actual bugs in components.
      (error) => {
        this.setState({
          isLoaded: true,
          error
        });
      })
  }


  render() {
    
    // const { error, isLoaded, LoadInvalidated, LoadValidated, } = this.state;
    if (!this.state.LoadValidated && ! this.state.LoadInvalidated) {
      return <div>Loading...</div>;
    } else {
      console.log("rendering start1")
      if (this.state.LoadValidated) {
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
      } else {
        return (
          <div className="login-backdrop">
            { User.getUserState() ? <Redirect to="/app/"/> : '' }
            <div className='login-container'>
              <Link to="/home"><img src={BackArrow} alt="Go Back" className="back-arrow"/></Link>
              <img src={Logo} alt="Logo" className="logo-image"/>
              <h2>Reset your password</h2>
              <p className="instructions-text">The link has expired. Please request a new password recovery email.</p>
            </div>
          </div>
        );
      }
    }
  }
}