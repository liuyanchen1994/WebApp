
import { Model } from "./Model"

export interface AddCatalogForm {

	//
	id: string

	//
	status: number

	//
	createdTime: Date

	//
	updatedTime: Date


	//
	name: string;

	//分类值
	value: string;

	//分类的类别
	type: string;

	//是否为顶级分类
	isTop: number;

	//上级/顶级(如果存在)分类
	topCatalogId: string;

}


