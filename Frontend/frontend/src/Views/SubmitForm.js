import React from "react";
import TextField from "material-ui/TextField";
import RaisedButton from "material-ui/RaisedButton";

export default class Form extends React.Component {
  state = {
    username: "",
    usernameError: "",
  };

  change = e => {
    // this.props.onChange({ [e.target.name]: e.target.value });
    this.setState({
      [e.target.name]: e.target.value
    });
  };

  validate = () => {
    let isError = false;
    const errors = {
      usernameError: "",
    };
    //this can be improved by looking at the database and find if there is a matching username.
    if (this.state.username.length < 5) {
      isError = true;
      errors.usernameError = "Username does not exist.";
    }
    //else if(this.state.username.id in the database){}

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
      this.props.onSubmit(this.state);
      // clear form
      this.setState({
        username: "",
        usernameError: "",
      });
    }
  };

  render() {
    return (
      <form>
        <TextField
          name="username"
          hintText="Username"
          floatingLabelText="Username"
          value={this.state.username}
          onChange={e => this.change(e)}
          errorText={this.state.usernameError}
          floatingLabelFixed
        />
        <br />
        <br />
        <RaisedButton label="Add" onClick={e => this.onSubmit(e)} primary />
        <br />
        <br />
        <br />
      </form>
    );
  }
}







    //DO NOT WORRY ABOUT THIS CODE.
        // onTextChanged =(e) => {
        //     const value = e.target.value;
        //     let suggestions = [];
        //     if (value.length > 0){
        //         const regex = new RegExp(`^${value}`,'i');
        //         suggestions = this.items.sort().filter(v => regex.test(v));
        //     }
        //     this.setState(()=>({ suggestions,text: value}));

        // }
        // renderSuggestion(){
        //     const {suggestions} = this.state;
        //     if(suggestions.length === 0){
        //         return null;
        //     }
        //     return (
        //         <ul> 
        //             {suggestions.map((item) => <li onClick ={() => this.suggestionSelected(item)}>{item}</li>)}
        //         </ul>
        //     )

        // }
        // suggestionSelected(value){
        //     this.setState(()=>({
        //         text: value,
        //         suggestions:[],
        //     }))

        // }