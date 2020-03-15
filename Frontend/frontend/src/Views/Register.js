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

    this.checkUsernameEmailUnique = this.checkUsernameEmailUnique.bind(this);
    this.createNewAccount = this.createNewAccount.bind(this);
    this.bindInput = this.bindInput.bind(this);
  }

  async checkUsernameEmailUnique (event) {
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
    
    let checkNewUsernameResult = await APIRequest.checkNewAccount(this.state.username, this.state.email);
    if (checkNewUsernameResult.ok) {
      this.setState({ firstSignUpPage: false });
    } else {
      switch (checkNewUsernameResult.status) {
        case 400:
          this.setState({ error: "Username already in use. "});
          break;
        case 409:
          this.setState({ error: "Email address already in use. "});
          break;
        default:
          this.setState({ error: "Unknown error (check your internet). "});
          break;
      }
    }
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
          <Link to={this.state.firstSignUpPage ? "/home" : "/register"} onClick={() => this.goBackToFirstPage()}>
            <img src={BackArrow} alt="Go Back" className="back-arrow"/>
          </Link>
          <img src={Logo} alt="Logo" className="logo-image"/>
          <h2>{ this.state.firstSignUpPage ? "Create a new account" : "Contact details" }</h2>
          <form id="form" action="#" method="POST">
            { this.state.firstSignUpPage && 
              <>
                <input type='text' name='email' onChange={this.bindInput} placeholder='Email*' defaultValue={this.state.email}/>
                <input type='text' name='username' onChange={this.bindInput} placeholder='Username*' defaultValue={this.state.username}/>
                <input type='text' name='password' onChange={this.bindInput} placeholder='Password*' defaultValue={this.state.password}/>
                <input type='text' name='passwordRepeat' onChange={this.bindInput} placeholder='Retype password*' defaultValue={this.state.passwordRepeat}/>
              </>
            }
            { !this.state.firstSignUpPage && 
              <>
                <input type='text' name='firstName' onChange={this.bindInput} placeholder='First name*'/>
                <input type='text' name='lastName' onChange={this.bindInput} placeholder='Last name*' />
                <input type='text' name='dateOfBirth' onChange={this.bindInput} placeholder='Date of birth*' />
                <input type='text' name='phoneNumber' onChange={this.bindInput} placeholder='Phone number' />
                <input type='text' name='bankAccountNumber' onChange={this.bindInput} placeholder='Bank account number' />
                <input type='text' name='medicalInfo' onChange={this.bindInput} placeholder='Medical information (i.e. allergies, etc)'/>
              </>
            }
            <input 
              type='submit' 
              value={this.state.firstSignUpPage ? 'Continue' : 'Create account'} 
              onClick={this.state.firstSignUpPage ? this.checkUsernameEmailUnique : this.createNewAccount} />
            { this.state.error ? <div className='login-error'> { this.state.error } </div> : <div className="error-placeholder"/> }
            <div className="other-actions">
              <p className="other-actions-text">Already a member? <Link to="/login" className="other-actions-link">Sign in.</Link></p>
            </div>
          </form> 
        </div>
      </div>
    );
  }
}