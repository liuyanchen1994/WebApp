
import { Model } from "./Model"

export interface AddResourceForm {


	//资源名称
	name: string;

	//说明及描述
	description: string;

	//资源类型 0 文档;1 视频;2 软件;3 代码
	type: ResourceType;

	//语言 0 中文;1 英文
	language: LanuageType;

	//图片地址
	imgUrl: string;

	//相对地址
	path: string;

	//绝对地址
	absolutePath: string;

	//分类
	catelogId: string;

}

enum ResourceType {
	ALL = 0,
	DOCUMENT = 1,
	VIDEO = 2,
	SOFTWARE = 3,
	CODE = 4,

};
enum LanuageType {
	CHINESE = 0,
	ENGLISH = 1,

};

