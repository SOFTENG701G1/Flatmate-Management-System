import React from 'react';
import { Redirect, Link } from 'react-router-dom';
import APIRequest from '../Util/APIRequest';
import "./Members.css";

export default class Members extends React.Component {
    constructor (props) {
      super(props);
  

      this.state = {
        items: [],
        isLoaded: false,
      }
      this.getMember();
    }
    /* Function used to fetch JSON data from the API */ 
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
            <div className = "MembersPage"> 
                <td className = "MembersCells">
                    <table class="MembersModule">
                        <td>
                            <div className = "user_profile"> </div>
                            <p> Name: {items[2].firstName} {items[2].lastName} </p>
                            <p> Email: {items[2].email} </p>
                            <p> Username: {items[2].userName}</p>
                        </td>
                    </table>
                </td>
                <td className = "MembersCells">
                    <table class="MembersModule">
                        <td>
                            <div className = "user_profile"> </div>
                            <p> Name: {items[1].firstName} {items[1].lastName} </p> 
                            <p> Email: {items[1].email} </p>
                            <p> Username: {items[1].userName}</p>
                        </td>
                    </table>
                </td>

                <td className = "MembersCells">
                    <table class="MembersModule">
                        <td>
                            <div className = "user_profile"> </div>
                            <p> Name: {items[0].firstName} {items[0].lastName} </p> 
                            <p> Email: {items[0].email} </p>
                            <p> Username: {items[0].userName}</p>
                        </td>
                    </table>
                </td>

                
            </div>);
        }
    }
}