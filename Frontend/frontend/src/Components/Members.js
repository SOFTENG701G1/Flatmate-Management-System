import React, {Component} from 'react';
import '../App.css';
import './Members.css';
import { Divider } from '@material-ui/core';
export default class MembersPage extends Component {

    constructor(props){
        super(props)
        this.state ={
            //dummy data
            userDetails: [{
                userName: "superman",
                fullName: "clolly dll",
                DOB: "03/04/1997",
                phoneNumber: "021020400203",
            }, {
                userName: "batman",
                fullName: "Daniel all",
                DOB: "03/12/1998",
                phoneNumber: "02102242403",
            }],

        }

    }
    
    handleSubmit = (event) => {
        event.preventDefault()
        const data = this.state
        this.setState({
            [event.target.name]: event.target.value 
         })
        console.log("final data ",data)

    }
    handleInputChange = (event) =>{
        event.preventDefault()
    }
    render () {
        const {userName} = this.state
        const members = []
        this.state.userDetails.forEach(
            users => {
                members.push(
                    <MemberProfiles userDetails={users} />
                )
            }
        )
        return (
        <div>
            <div className='section-header'>
                Members page
            </div>
            <div>            
                <form onSubmit = {this.handleSubmit}>            
                <input type = 'text' name = 'userName' className = 'Usernamebox' placeholder='Enter Username'/>
                <button type = 'Submit' className = "button">Add</button>
                </form>
            </div>
            <div><h4 className = 'currentMember'>Current Members</h4></div>
            <div>{members}</div>
            
        </div>
        )
    }
}

function MemberProfiles(props){
    const userName = props.userDetails.userName;
    const fullName = props.userDetails.fullName;
    const DOB = props.userDetails.DOB;
    const phoneNumber =props.userDetails.phoneNumber;
    return (
        <div className="MemberProfile">
            {userName}
            <br></br>
            {fullName}
            <br></br>
            DOB: {DOB}
            <br></br>
            Phone number: {phoneNumber}
            
        </div>
        
    )
}