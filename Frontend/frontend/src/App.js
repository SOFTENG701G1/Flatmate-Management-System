import React from 'react';
import logo from './logo.png';
import './App.css';
import TestComponent from './Components/TestComponent';

export default class App extends React.Component {
  render () {
    return (
      <div className="App">
        <div className="container center">
        <h1>Flat Management System</h1>
        <nav className="menu">

            <div className="menu__down">
                <ul className="menu__list">
                
                    <li className="menu__logo"></li>
                    <li className="menu__list-item"><a className="menu__link" href="#">Name</a></li>
                    <li className="menu__list-item"><a className="menu__link" href="#">Dashboard</a></li>
                    <li className="menu__list-item"><a className="menu__link" href="#">Payments</a></li>
                    <li className="menu__list-item"><a className="menu__link" href="#">Members</a></li>
                </ul>

            </div>
        </nav>
    </div>
      </div>
    );
  }
}
