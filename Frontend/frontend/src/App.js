import React from 'react';
import './App.css';
import Payments from './Components/Payments';

export default class App extends React.Component {
  render () {
    return (
      <div className="App">
        <header className="App-header">
          <Payments></Payments>
        </header>
      </div>
    );
  }
}
