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
               
            <div className='section-header'>Members page</div>        
            <input type = 'text'/>
            <button className = "button">Add</button>

        </div>
        </MuiThemeProvider>
        );
    }
}