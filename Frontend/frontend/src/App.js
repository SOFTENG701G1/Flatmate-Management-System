import React from 'react';
import logo from './logo.png';
import './App.css';
import TestComponent from './Components/TestComponent';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link
} from "react-router-dom";

export default class App extends React.Component {
  render() {
    return (
      <div className="App">
        <div className="container center">
          <Router>
            <Route path="/login">
              Login
            </Route>
            <Route path="/register">
              Register
            </Route>
            <Route path="/app/*">
              <nav className="menu">
                <div className="menu__down">
                  <ul className="menu__list">
                    <li className="menu__logo"></li>
                    <li className="menu__list-item"><Link to="/app/"><a className="menu__link" href="#">Home</a></Link></li>
                    <li className="menu__list-item"><Link to="/app/dashboard"><a className="menu__link" href="#">Dashboard</a></Link></li>
                    <li className="menu__list-item"><Link to="/app/payments"><a className="menu__link" href="#">Payments</a></Link></li>
                    <li className="menu__list-item"><Link to="/app/members"><a className="menu__link" href="#">Members</a></Link></li>
                  </ul>
                </div>
              </nav>
              <Switch>
                <Route path="/app/dashboard">
                  <Dashboard />
                </Route>
                <Route path="/app/payments">
                  <Payments />
                </Route>
                <Route path="/app/members">
                  <Members />
                </Route>
                <Route path="/app/">
                  <Home />
                </Route>
              </Switch>
            </Route>
          </Router>
        </div>
      </div>
    );
  }
}
/*Router is linked to functions for the time being*/
function Home() {
  return <h2>Flat Management System</h2>;
}

function Dashboard() {
  return <h2>Dashboard page</h2>;
}

function Payments() {
  return <h2>Payments page</h2>;
}
function Members() {
  return <h2>Members page</h2>;
}
