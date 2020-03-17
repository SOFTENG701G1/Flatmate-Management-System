import React from 'react';
import { Redirect, Link } from 'react-router-dom';

import './Login.css';
import APIRequest from '../Util/APIRequest';
import User from '../Util/User';
import Logo from '../images/logo-house-blue.png';
import BackArrow from '../images/back-arrow.png';

export default class Login extends React.Component {
  constructor (props) {
    super(props);

    this.state = {
      username: undefined,
      password: undefined
    }

    this.login = this.login.bind(this);
    this.bindInput = this.bindInput.bind(this);
  }

  async login (event) {
    event.preventDefault();
    this.setState({ error: ""});
    if (!this.state.username || !this.state.password) {
      this.setState({ error: "Username and password are required."});
      return;
    }
    
    let loginResult = await APIRequest.login(this.state.username, this.state.password);

    if (loginResult.ok) {
      User.setUserState(await loginResult.json());
      this.forceUpdate(); // Triggers re-render, which will activate redirect now user is setup
    } else {
      switch (loginResult.status) {
        case 404:
          this.setState({ error: "Username does not exist."});
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

  render() {
    // TODO: Add forgot password url to link
    return (
      <div className="login-backdrop">
        { User.getUserState() ? <Redirect to="/app/"/> : '' }
        <div className='login-container'>
          <Link to="/home"><img src={BackArrow} alt="Go Back" className="back-arrow"/></Link>
          <img src={Logo} alt="Logo" className="logo-image"/>
          <h2> Login to your account </h2>
          <form action="#" method="POST">
            <input type='text' name='username' onChange={this.bindInput} placeholder='Username'/>
            <input type='password' name='password' className="input-field" onChange={this.bindInput} placeholder='Password'/>
            <input type='submit' value='Login' onClick={this.login}/>
            { this.state.error ? <div className='login-error'> { this.state.error } </div> : <div className="error-placeholder"/> }
            <div className="other-actions">
              <p className="other-actions-text">Not a memeber? <Link to="/register" className="other-actions-link">Sign up!</Link></p>
              <Link to="/login/forgot-password" className="other-actions-link">Forgot your password?</Link>
            </div>
          </form>
        </div>
      </div>
    );
  }
}