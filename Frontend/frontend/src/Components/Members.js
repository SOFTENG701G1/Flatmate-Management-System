import React, {Component} from 'react';
import '../App.css';

export default class MembersPage extends Component {

    constructor(props){
        super(props);
        this.items =[
            'david','damien','sara','jane',
        ];

        this.state ={
            suggestions: [],
            text:'',
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
            <div>
                <h2>Members Page</h2>
                <p>Add member <input value = {text} onChange= {this.onTextChanged} type = "text"/>
                </p>
                
                {this.renderSuggestion()}
            </div>
        );
    }
}