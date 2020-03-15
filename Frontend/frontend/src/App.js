import React from 'react';
import './App.css';
import Dashboard from './Components/Dashboard';
import Chores from './Components/Chores';
import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";
import Dashboard from './Components/Dashboard';
import Navigation from './Components/Navigation';
import Login from './Views/Login';
import Logout from './Views/Logout';
import HomePage from './Views/HomePage';
import RequireLogin from './Components/RequireLogin';
import MembersPage from './Components/Members';
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
                  <MembersPage/>
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

function Payments() {
  return <h2>Payments page</h2>;
}
