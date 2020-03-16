import React, {Component} from 'react';
import '../App.css';
import Form from "./SubmitForm";
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
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
    onTextChanged =(e) => {
      const value = e.target.value;
      let suggestions = [];
      if (value.length > 0){
          const regex = new RegExp(`^${value}`,'i');
          suggestions = this.items.sort().filter(v => regex.test(v));
      }
      this.setState(()=>({ suggestions,text: value}));

  }
  renderSuggestion(){
      const {suggestions} = this.state;
      if(suggestions.length === 0){
          return null;
      }
      return (
          <ul> 
              {suggestions.map((item) => <li onClick ={() => this.suggestionSelected(item)}>{item}</li>)}
          </ul>
      )

  }
  suggestionSelected(value){
      this.setState(()=>({
          text: value,
          suggestions:[],
      }))

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
                <Form onSubmit={submission =>this.setState({data: [...this.state.data, submission]})}/>
            </div>
            </MuiThemeProvider>
        );
    }
}