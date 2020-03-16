import React, {Component, useState} from 'react';
import '../App.css';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import Select from '@material-ui/core/Select';

function ChoresUpdateButton(){
  const [popupState, setPopupState] = React.useState(false);

  const handleOpenPopup = () => {
    setPopupState(true);
  };
  const handleClosePopup = () => {
    setPopupState(false);
  };

  return (
    <div>
      <Button style={{"background-color":"#1F5673", "color": "white"}} onClick={handleOpenPopup}>
        Add Chore
      </Button>
      <Dialog open={popupState} onClose={handleClosePopup} aria-labelledby="form-dialog-title">
        <DialogTitle id="form-dialog-title">Add Chore</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Use following form to assign people or yourself chores. You can see your assigned chores
            on the dashboard
          </DialogContentText>
          <TextField
            autoFocus
            margin="dense"
            id="title"
            label="Title"
            fullWidth
          />
          <div className="ChoresFlatMembers">
            <InputLabel  id="demo-simple-select-label">Assigned Flat Members</InputLabel>
            <Select
            style={{color: 'red'}}
              labelId="demo-simple-select-label"
              id="demo-simple-select"
              
            >
              <MenuItem >Ten</MenuItem>
              <MenuItem >Twenty</MenuItem>
              <MenuItem >Thirty</MenuItem>
            </Select>
          </div>
          <TextField
            autoFocus
            margin="dense"
            id="description"
            label="Description"
            fullWidth
            multiline
            rows="4"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClosePopup} color="primary">
            Cancel
          </Button>
          <Button onClick={handleClosePopup} color="primary">
            Add
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}

export default ChoresUpdateButton;