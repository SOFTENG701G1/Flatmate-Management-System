import React from "react";
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';
import { makeStyles } from '@material-ui/core/styles';




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
        <div>
        <TextField
          id="outlined-basic"
          name="username"
          hintText="Username"
          floatingLabelText="Username"
          value={this.state.username}
          onChange={e => this.change(e)}
          errorText={this.state.usernameError}
          floatingLabelFixed
          
        />
        <Button variant="contained" color="secondary" onClick={e => this.onSubmit(e)}>
        Add
        </Button>
        {/* <Button label="Add" onClick={e => this.onSubmit(e)} /> */}
        </div>
      </form>
    );
  }
}
