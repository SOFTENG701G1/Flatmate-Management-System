import React from 'react';
import { Link } from 'react-router-dom';

import './Login.css';

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

  login (event) {
    event.preventDefault();
    console.log(this.state.username);
    console.log(this.state.password);
  }
  
  bindInput (event) {
    let target = event.target;

    this.setState({
      [target.name]: target.value
    });
  }

  render() {
    return <div className="login-container">
        <div className='login-backdrop'></div>
        <div className='login-icon'></div>
        <h2> Login to your Account </h2>
        <form action="#" method="POST">
          <input type='text' name='username' onChange={this.bindInput} placeholder='Username'/>
          <input type='password' name='password' onChange={this.bindInput} placeholder='Password'/>
          <input type='submit' value='Login' onClick={this.login}/>
        </form>
      </div>
  }
}