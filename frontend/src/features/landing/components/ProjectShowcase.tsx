import { ProjectListDto } from "@/features/project/types";
import { faker } from '@faker-js/faker';
import React, { useState } from "react";
import clsx from "clsx";
import '../styles/projectShowcase.pcss'
import { types } from "mobx-state-tree";

faker.seed(123);

const getFakeProjectList: () => ProjectListDto = () => ({
  id: faker.datatype.number(),
  name: faker.commerce.productName(),
  shortName: faker.commerce.productName(),
  description: faker.lorem.paragraphs(3, '\n\n'),
  photo: {
    id: 1,
    path: `https://picsum.photos/seed/${faker.datatype.number()}/450/230`
  },
  state: {
    id: 1,
    name: "В работе",
    color: "gr-green",
  },
  tags: [{
    id: 1,
    name: "Tag 1"
  }, {
    id: 2,
    name: "Tag 2"
  }],
  team: {
    id: 1,
    name: faker.commerce.product(),
    photo: {
      id: 1,
      path: "https://picsum.photos/450/230"
    }
  }
});

type SelectedProjectProps = {
  project: ProjectListDto;
}

const SelectedProject = ({ project }: SelectedProjectProps) => (
  <div className="projectshower__selected">
    <p className="projectshower__selected__ribbon">НОВОЕ</p>
    <img className="projectshower__selected__backgorund" src={project.photo.path} alt={project.shortName}/>

    <div className="projectshower__selected__inner-info">
      <p className="projectshower__selected__heading">
        {project.shortName}
      </p>
      <p className="projectshower__selected__description">
        {project.description}
      </p>
      <a className="projectshower__selected__action" href="#">Перейти</a>
    </div>
  </div>
);

type ProjectShowerListItemProps = {
  project: ProjectListDto;
  onClick: () => void;
  highlighted: boolean;
}


const ProjectShowerListItem: React.FC<ProjectShowerListItemProps> = ({ project, onClick, highlighted }) => {
  return (
    <div className={clsx("projectshower__list-item", { 'projectshower__list-item_selected': highlighted })}
         onClick={() => onClick()}
    >
      <img className="projectshower__list-item__photo" src={project.photo.path} alt={project.shortName}/>
      <p className="projectshower__list-item__name">
        {project.shortName}
      </p>
    </div>)
}

const data = [...Array(4).keys()].map(() => getFakeProjectList());

const testStore = types.model("TestStore", {
  name: types.string,
  age: types.number,
}).actions(self => ({
  setName(name: string) {
    self.name = name;
  },
  setAge(age: number) {
    self.age = age;
  }
}));


export const ProjectShowcase = () => {
  const [selectedProject, setSelectedProject] = useState<ProjectListDto>(data[0]);

  return (
    <div className="projectshower">
      <SelectedProject project={selectedProject}/>
      <div className="projectshower__list">
        {data.map((project, index) => (
          <ProjectShowerListItem
            key={project.id}
            project={project}
            onClick={() => setSelectedProject(project)}
            highlighted={selectedProject.id === project.id}
          />))
        }
      </div>
    </div>
  )
}
