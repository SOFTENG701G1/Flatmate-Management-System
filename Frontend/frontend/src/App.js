import React from 'react';
import './App.css';
import Chores from './Components/Chores';
import Payments from './Components/Payments';
import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";
import Dashboard from './Components/Dashboard';
import Navigation from './Components/Navigation';
import Login from './Views/Login';
import Register from './Views/Register';
import Logout from './Views/Logout';
import SplashScreen from './Views/SplashScreen';
import HomePage from './Views/HomePage';
import RequireLogin from './Components/RequireLogin';
import APIRequest from './Util/APIRequest';
import NewFlat from './Components/NewFlat';
import MembersPage from './Components/Members';
import ForgotPassword from './Views/ForgotPassword';

export default class App extends React.Component {
  render() {
    return (
      <div className="App">
        <div className="container center">
          <Router>
            <Route exact path="/">
              <SplashScreen/>
            </Route>
            <Route path="/home">
              <HomePage/>
            </Route>
            <Route path="/login">
              <Login/>
            </Route>
            <Route path="/login/forgot-password">
              <ForgotPassword/>
            </Route>
            <Route path="/register">
              <Register/>
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
                <Route path="/app/newFlat">
                  <NewFlat />
                </Route>
                <Route path="/app/">
                  <Home/>
                </Route>
              </Switch>
            </Route>
          </Router>
        </div>
      </div>
    );
  }
}

/* Router is linked to functions for the time being */
function Home() {
  return <div class='section-header'>Flat Management System</div>;
}

