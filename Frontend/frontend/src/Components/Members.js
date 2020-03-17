import React, {Component} from 'react';
import '../App.css';
import './Members.css';
import AddMember from './AddMember';
export default class MembersPage extends Component {

    constructor(props){
        super(props)
    }

  
    render () {
        return (
          <div>
            <div className='section-header'>
                Members page
            </div>
            <AddMember/>
            <h4 className = 'currentMember'>Current Members</h4>
          </div>  
        )
    }
}
