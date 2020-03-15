import React from 'react';
import { Link } from 'react-router-dom';

import './Login.css';
import APIRequest from '../Util/APIRequest';
import User from '../Util/User';
import Logo from '../images/logo-house-blue.png';
import BackArrow from '../images/back-arrow.png';

export default class Register extends React.Component {
  constructor (props) {
    super(props);

    this.state = {
      firstSignUpPage: true,
      email: undefined,
      username: undefined,
      password: undefined,
      passwordRepeat: undefined,
      firstName: undefined, 
      lastName: undefined,
      dateOfBirth: undefined,
      phoneNumber: undefined,
      backAccountNumber: undefined,
      medicalInfo: undefined,
    }

    this.checkNewUsername = this.checkNewUsername.bind(this);
    this.createNewAccount = this.createNewAccount.bind(this);
    this.bindInput = this.bindInput.bind(this);
  }

  async checkNewUsername (event) {
    event.preventDefault();
    this.setState({ error: ""});
    if (!this.state.email || !this.state.username || !this.state.password || !this.state.passwordRepeat) {
      this.setState({ error: "All fields are required."});
      return;
    }

    if (this.state.password !== this.state.passwordRepeat) {
      this.setState({ error: "Passwords do not match." });
      return;
    }

    this.setState({ firstSignUpPage: false }); // TODO: Remove once API Request implemented
    document.forms["form"].reset();
    
    // let checkNewUsernameResult = await APIRequest.checkNewUsername(this.state.username);

    // if (checkNewUsernameResult.ok) {
    //   this.setState({ firstSignUpPage: false });
    // } else {
    //   switch (checkNewUsernameResult.status) {
    //     // TODO: Change switch statements to display username already exists error and unknown error (check your internet)
    //     case 404:
    //       this.setState({ error: "There's no such user. "});
    //       break;
    //     case 403:
    //       this.setState({ error: "Invalid password. "});
    //       break;
    //     default:
    //       this.setState({ error: "Unknown error (check your internet). "});
    //       break;
    //   }
    // }
  }
  async createNewAccount (event) {
    event.preventDefault();
    this.setState({ error: ""});
    if (!this.state.firstName || !this.state.lastName || !this.state.dateOfBirth) {
      this.setState({ error: "First name, last name and date of birth is required."});
      return;
    }

    // TODO: Create account here
  }
  
  bindInput (event) {
    let target = event.target;

    this.setState({
      [target.name]: target.value
    });
  }

  goBackToFirstPage = () => {
    this.setState({ firstSignUpPage: true });
  }

  render() {
    return (
      <div className="login-backdrop">
        <div className='login-container'>
          <Link to={this.state.firstSignUpPage ? "/" : "/register"} onClick={() => this.goBackToFirstPage()}>
            <img src={BackArrow} alt="Go Back" className="back-arrow"/>
          </Link>
          <img src={Logo} alt="Logo" className="logo-image"/>
          <h2>{ this.state.firstSignUpPage ? "Create a new account" : "Contact details" }</h2>
          { this.state.firstSignUpPage ? 
            <form id="form" action="#" method="POST">
              <input type='text' name='email' onChange={this.bindInput} placeholder='Email*'/>
              <input type='text' name='username' onChange={this.bindInput} placeholder='Username*'/>
              <input type='text' name='password' onChange={this.bindInput} placeholder='Password*'/>
              <input type='text' name='passwordRepeat' onChange={this.bindInput} placeholder='Retype password*'/>
              <input type='submit' value='Continue' onClick={this.checkNewUsername} defaultValue=""/>
              { this.state.error ? <div className='login-error'> { this.state.error } </div> : <div className="error-placeholder"/> }
              <div className="other-actions">
                <p className="other-actions-text">Already a member? <Link to="/login" className="other-actions-link">Sign in.</Link></p>
              </div>
            </form> 
            : 
            <form action="#" method="POST">
              <input type='text' name='firstName' onChange={this.bindInput} placeholder='First name*' defaultValue=""/>
              <input type='text' name='lastName' onChange={this.bindInput} placeholder='Last name*' defaultValue=""/>
              <input type='text' name='dateOfBirth' onChange={this.bindInput} placeholder='Date of birth*' defaultValue=""/>
              <input type='text' name='phoneNumber' onChange={this.bindInput} placeholder='Phone number' defaultValue=""/>
              <input type='text' name='bankAccountNumber' onChange={this.bindInput} placeholder='Bank account number' defaultValue=""/>
              <input type='text' name='medicalInfo' onChange={this.bindInput} placeholder='Medical information (i.e. allergies, etc)'/>
              <input type='submit' value='Create account' onClick={this.createNewAccount}/>
              { this.state.error ? <div className='login-error'> { this.state.error } </div> : <div className="error-placeholder"/> }
              <div className="other-actions">
                <p className="other-actions-text">Already a member? <Link to="/login" className="other-actions-link">Sign in.</Link></p>
              </div>
            </form> 
          }
        </div>
      </div>
    );
  }
}