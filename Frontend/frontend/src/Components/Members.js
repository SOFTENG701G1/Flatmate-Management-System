import React, {Component} from 'react';
import '../App.css';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
export default class MembersPage extends Component {

    render () {
        return (
          <MuiThemeProvider>
            <div>
               
            <div className='section-header'>Members page</div>        
            <input type = 'text'/>
            <button className = "button">Add</button>

        </div>
        </MuiThemeProvider>
        );
    }
}