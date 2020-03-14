import React from 'react';
import { Link } from 'react-router-dom';

import './Login.css';

export default class Login extends React.Component {
  render() {
    return <div className="login-container">
        <div className='login-backdrop'></div>
        <div className='login-icon'></div>
        <h2> Login to your Account </h2>
        <input type='text' placeholder='Username'/>
        <input type='text' placeholder='Password'/>
        <input type='button' value='Login'/>
      </div>
  }
}