import React, { Component } from "react";
import {
  Dialog,
  Fab,
  TextField,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Button,
  Checkbox,
  FormControlLabel,
} from "@material-ui/core";
import AddIcon from "@material-ui/icons/Add";

class ChoresDialog extends Component {
  constructor(props) {
    super(props);

    this.state = {
      open: false,
      chore: {
        title: "",
        description: "",
        assignee: "",
        due_date: "",
        recurring: false,
        completed: 0,
      },
    };
  }

  handleClickOpen() {
    this.setState({
      open: true,
    });
  }

  handleClickClose() {
    this.setState({
      open: false,
    });
  }

  handleChange(event) {
    console.log(event.target.value);
    const { chore } = this.state;
    this.setState({
      chore: {
        ...chore,
        [event.target.name]: event.target.value
          ? event.target.value
          : event.target.checked,
      },
    });
  }

  handleSubmit() {
    // do somehign
    console.log(this.state.chore);
    this.handleClickClose();
  }

  render() {
    const { open } = this.state;
    return (
      <div>
        <Fab onClick={() => this.handleClickOpen()}>
          <AddIcon />
        </Fab>
        <Dialog open={open} onClose={() => this.handleClickClose()}>
          <DialogTitle id="form-dialog-title">New Chore</DialogTitle>
          <DialogContent>
            <DialogContentText>Add a new chore</DialogContentText>
            <form onSubmit={() => this.handleSubmit()}>
              <TextField
                autoFocus
                margin="dense"
                id="name"
                label="Title"
                name="title"
                fullWidth
                onChange={(event) => this.handleChange(event)}
              />
              <TextField
                label="Description"
                name="description"
                onChange={(event) => this.handleChange(event)}
              ></TextField>
              <TextField
                label="Assignee"
                name="assignee"
                onChange={(event) => this.handleChange(event)}
              ></TextField>
              <TextField
                id="datetime-local"
                label="Next appointment"
                type="datetime-local"
                name="due_date"
                defaultValue="2017-05-24T10:30"
                InputLabelProps={{
                  shrink: true,
                }}
                onChange={(event) => this.handleChange(event)}
              />
              <FormControlLabel
                control={<Checkbox color="primary" />}
                value={this.checked}
                label="Recurring"
                labelPlacement="start"
                name="recurring"
                onChange={(event) => this.handleChange(event)}
              />
            </form>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => this.handleClickClose()} color="primary">
              Cancel
            </Button>
            <Button
              type="submit"
              onClick={() => this.handleSubmit()}
              color="primary"
              value="submit"
            >
              Submit
            </Button>
          </DialogActions>
        </Dialog>
      </div>
    );
  }
}
export default ChoresDialog;
