import React, {Component} from 'react';
import '../App.css';
import './Members.css';
export default class MembersPage extends Component {

    constructor(props){
        super(props)
        this.state ={
            //dummy data
            userNames: []

        }

    }
    
    addUser(e){
        e.preventDefault();
        const {userNames} = this.state;
        const newUser = this.newUser.value;
        this.setState({
            userNames: [...this.state.userNames, newUser]
        })
    }
    
    render () {
        const {userNames} = this.state
        return (
        <div>
            <div className='section-header'>
                Members page
            </div>
                            
                <form onSubmit = { (e) => {this.addUser(e)}}>    
                <div>        
                <input ref = {input => this.newUser = input}type = 'text' name = 'userName' className = 'Usernamebox' placeholder='Enter Username'/>
                </div>
                <button type = 'submit' className = "button" >Add</button>
                </form> 
            <div>
            <h4 className = 'currentMember'>Current Members</h4></div>
                <div>
                    {userNames.map(user =>{
                        return <div className ="MemberProfile">{user}</div>
                            
                    })
                }
                </div>
            </div>
        )
    }
}
