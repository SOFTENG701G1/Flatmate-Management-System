import React from 'react';
import './App.css';
import Dashboard from './Components/Dashboard';
import Chores from './Components/Chores';

import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";
import Navigation from './Components/Navigation';
import Login from './Views/Login';
import Logout from './Views/Logout';
import HomePage from './Views/HomePage';
import RequireLogin from './Components/RequireLogin';

export default class App extends React.Component {
  render() {
    return (
      <div className="App">
        <div className="container center">
          <Router>
            <Route exact path="/">
              <HomePage/>
            </Route>
            <Route path="/login">
              <Login/>
            </Route>
            <Route path="/register">
              Register
            </Route>
            <Route path="/logout">
              <Logout/>
            </Route>
            <Route path="/app/*">
              <RequireLogin/>
              <Navigation/>
              <Switch>
                <Route path="/app/dashboard">
                  <Dashboard />
                </Route>
                <Route path="/app/chores">
                  <Chores />
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
  return <div class='section-header'>Flat Management System</div>;
}

function Payments() {
  return <div class='section-header'>Payments page</div>;
}
function Members() {
  return <div class='section-header'>Members page</div>;
}
