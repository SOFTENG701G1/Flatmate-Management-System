import React from 'react';
import { Link } from 'react-router-dom';

import './Navigation.css';

export default class Navigation extends React.Component {
  render() {
    return <nav className="menu">
      <div className="menu__down">
        <ul className="menu__list">
          <li className="menu__logo"></li>
          <li className="menu__logout">
            <Link to="/logout/"><a className="menu__link" href="#">Logout</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/"><a className="menu__link" href="#">Home</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/dashboard"><a className="menu__link" href="#">Dashboard</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/payments"><a className="menu__link" href="#">Payments</a></Link>
          </li>
          <li className="menu__list-item">
            <Link to="/app/members"><a className="menu__link" href="#">Members</a></Link>
          </li>
        </ul>
      </div>
    </nav>
  }
}