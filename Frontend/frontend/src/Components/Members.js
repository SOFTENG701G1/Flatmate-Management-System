import React from 'react';
import { Redirect, Link } from 'react-router-dom';
import APIRequest from '../Util/APIRequest';

export default class Members extends React.Component {
    constructor (props) {
      super(props);
  

      this.state = {
        items: [],
        isLoaded: false,
      }
    }
    async getMember(){
        const memberResult = await APIRequest.componentDidMount()
        const json = await memberResult.json();
        this.setState({
                    isLoaded: true,
                    items: json
                })
        
    }
    render() {
        var {isLoaded, items} = this.state;

        if (!isLoaded){
            return (<div>Loading...</div>);
        }
        else{
            return (
            <div className = "Members"> 
                <ul>
                    {items.map(item =>(
                        <li key={item.userName}>
                            Name: {item.firstName} | Email: {item.email}   
                        </li>
                    ))}
                </ul>
            </div>);
        }
    }
}