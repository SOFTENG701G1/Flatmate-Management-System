import React from 'react';
import './App.css';
import Dashboard from './Components/Dashboard';
import Chores from './Components/Chores';
import Payments from './Components/Payments';

import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";
<<<<<<< HEAD
 import Members from './Components/Members';
=======
import Navigation from './Components/Navigation';
import Login from './Views/Login';
import Register from './Views/Register';
import Logout from './Views/Logout';
import SplashScreen from './Views/SplashScreen';
import HomePage from './Views/HomePage';
import RequireLogin from './Components/RequireLogin';

>>>>>>> 50-display-members-of-flat
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

<<<<<<< HEAD
function Dashboard() {
  return <h2>Dashboard page</h2>;
}

function Payments() {
  return <h2>Payments page</h2>;
}

=======
function Members() {
  return <div class='section-header'>Members page</div>;
}
>>>>>>> 50-display-members-of-flat
