import React, {Component} from 'react';
import '../App.css';
import {
    Table,
    TableBody,
    TableHeader,
    TableHeaderColumn,
    TableRow,
    TableRowColumn,
  } from 'material-ui/Table';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider'
import TableEx from '../Views/Table.js';
import Form from "../Views/SubmitForm";
export default class MembersPage extends Component {

    constructor(props){
        super(props);
        this.items =[
            'david','damien','sara','jane',
        ];
        this.state ={
            suggestions: [],
            text:'',
            data : [],
        };


    }

    render () {
        const {text} = this.state;
        return (
            <MuiThemeProvider>
            <div>
                <h2>Members Page</h2>
                {/* <input value = {text} onChange= {this.onTextChanged} type = "text" placeholder = "Enter Username to add and press Enter"/>
                <button type = "submit">Add User to flat</button>
                {this.renderSuggestion()} */}
                <Form
            onSubmit={submission =>
              this.setState({
                data: [...this.state.data, submission]
              })}
          />
          <TableEx 
            data={this.state.data}
            header={[
              {
                name: "Username",
                prop: "username"
              },
              {
                name: "First Name",
                prop: "firstName"
              },
              {
                name: "Last name",
                prop: "lastName"
              },
              {
                name: "Email",
                prop: "email"
              }
            ]}
          />

            </div>
            </MuiThemeProvider>
        );
    }
}