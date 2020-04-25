import React from 'react';
import { Link } from 'react-router-dom';

import './Navigation.css';
import User from '../Util/User';

export default class Navigation extends React.Component {

  render() {
    return <nav className="menu">
      <div className="menu__down">
        <ul className="menu__list">
          <li className="menu__list-item menu__logo"></li>
          <li className="menu__list-item menu__user_detail">
          <div onClick={this.showModal}>
            <div className='user_profile'> </div>
            <div className='user_profile_name'>
              <Link to="/app/profile">
              <a className="menu__link" href="/app/profile">
              { User.getUserState() ? User.getUserState().userName : '' }
              </a>
              </Link>
            </div>
          </div>
          </li>
          <li className="menu__list-item">
            <Link to="/app/dashboard"><a className="menu__link" href="/app/dashboard">Dashboard</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/chores"><a className="menu__link" href="/app/chores">Chores</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/payments"><a className="menu__link" href="/app/payments">Payments</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/members"><a className="menu__link" href="/app/members">Members</a></Link>
          </li>
          <li className="menu__list-item menu__logout">
            <Link to="/logout/"><a className="menu__link" href="/logout">Logout</a></Link>
          </li>
        </ul>
      </div>
    </nav>
  }
}