import React, {Component} from 'react';
import APIRequest from "../Util/APIRequest";
export default class MembersPage extends Component {
    constructor(props){
        super(props)
        this.state ={
            userName: "",
            userNameError: ""
        }

    }

  
    change = e => {
        // this.props.onChange({ [e.target.name]: e.target.value });
        this.setState({
          [e.target.name]: e.target.value
        });
      
      };
    
      onSubmit = e => {
        e.preventDefault();
          //this.props.onSubmit(this.state);
          // clear form
          this.addMember().then((val) => this.userNameError = String(val.resultCode));
          this.setState({
            userName: ""
          });
      };

    async addMember(){
      const memberResult = await APIRequest.addMember(this.state.userName);
      return memberResult.json();
    };
    
    addUser(e){
        e.preventDefault();
        this.setState({
            [e.target.name]: e.target.value
        });
        console.log(e.target.value)
    }

    render () {
        return (
            <div>
            <form>       
            <input type = 'text' name = 'userName' onChange={e => this.change(e)}
            className = 'Usernamebox' placeholder='Enter Username'/>
            <button type = 'submit' className = "button" onClick = { (e) => {this.onSubmit(e)}} >Add</button>   
            </form> 
            <p className = 'error'>{this.state.userNameError}</p>
            </div>
        );
    }

}

