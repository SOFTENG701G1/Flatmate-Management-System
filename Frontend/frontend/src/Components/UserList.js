import React, {Component} from 'react';
import Checkbox from '@material-ui/core/Checkbox';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/row';
import './UserList.css'

export default class UserList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            checkedItems: [],
        };

        this.updateStateList = this.updateStateList.bind(this);
    }

    updateStateList(e, value){
        if (e.target.checked){
          this.setState({
            checkedItems: this.state.checkedItems.concat([value])
          }, () => {
              this.props.onListChange(this.state.checkedItems);
          })
        } else {
          this.setState({
            checkedItems : this.state.checkedItems.filter(function(val) {return val!==value})
          }, () => {
              this.props.onListChange(this.state.checkedItems);
          })
       }

    }

    render () {
        return (
            <React.Fragment>
            <Container className="UserContainer">
                 <Row className="UserListHeader">{this.props.title}</Row>
                 <Container className="UserListContainer">
                 {
                this.props.listItems.map((item,index) => (
                    <Row className="UserListRow">
                  <label key={index} >
                    <Checkbox name={item} onChange={(e)=>this.updateStateList(e,item)} />
                    {item}
                  </label>
                  </Row>
                ))
                }
                 </Container>
            </Container>
            </React.Fragment>
        );
    }
}