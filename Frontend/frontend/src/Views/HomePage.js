import React from 'react';
import { Redirect } from 'react-router-dom';

import User from '../Util/User';

import './Login.css';
import Logo from '../images/logo-house-blue.png';

export default class Login extends React.Component {
  state = {
    redirectTo: null,
  }

  login = () => {
    this.setState({ redirectTo: '/login' });
  }

  signUp = () => {
    this.setState({ redirectTo: '/register' });
  }

  render() {
    return (
      <div className="login-backdrop">
        { User.getUserState() && <Redirect to="/app/"/> }
        { this.state.redirectTo && <Redirect to={this.state.redirectTo}/> }
        <div className='login-container'>
          <img src={Logo} alt="logo" className="logo-image"/>
          <h2> Login or sign up </h2>
          <form action="#" method="POST" style={{ marginTop: 20 }}>
            <input type='submit' value='Login' onClick={this.login}/>
            <input type='submit' value='Sign up' onClick={this.signUp}/>
          </form>
        </div>
      </div>
    );
  }
}