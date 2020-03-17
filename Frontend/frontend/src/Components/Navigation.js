import React from 'react';
import { Link, Redirect } from 'react-router-dom';

import './Navigation.css';
import User from '../Util/User';

export default class Navigation extends React.Component {
  render() {
    return <nav className="menu">
      <div className="menu__down">
        <ul className="menu__list">
          <li className="menu__list-item menu__logo"></li>
          <li className="menu__list-item menu__user_detail">
            <div className='user_profile'> </div>
            <div className='user_profile_name'>
              { User.getUserState() ? User.getUserState().userName : '' }
            </div>
          </li>
          <li className="menu__list-item">
            <Link to="/app/"><a className="menu__link" href="#">Home</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/dashboard"><a className="menu__link" href="#">Dashboard</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/chores"><a className="menu__link" href="#">Chores</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/payments"><a className="menu__link" href="#">Payments</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/members"><a className="menu__link" href="#">Members</a></Link>
          </li>
          <li className="menu__list-item menu__logout">
            <Link to="/logout/"><a className="menu__link" href="#">Logout</a></Link>
          </li>
        </ul>
      </div>
    </nav>
  }
}