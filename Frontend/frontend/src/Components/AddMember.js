import React, {Component} from 'react';
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
        console.log(e.target.value)
      };
    
      validate = () => {
        let isError = false;
        const errors = {
          userNameError: "",
        };
    
        if (this.state.userName !== "test") {
          isError = true;
          errors.userNameError = "Username does not match to the system";
        }
        
        this.setState({
          ...this.state,
          ...errors
        });
        return isError;
      };
    
      onSubmit = e => {
        e.preventDefault();
        const err = this.validate();
        if (!err) {
          //this.props.onSubmit(this.state);
          // clear form
          this.setState({
            userName: "",
            userNameError: "",
          });
        }
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

